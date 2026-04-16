using Cysharp.Threading.Tasks;
using MyGame;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayfabService : MonoBehaviour
{
    public static PlayfabService instance;

    private string _titleGameId = "ED709";

    private GetUserInventoryResult _userInventory;
    public GetUserInventoryResult UserInventory => _userInventory;


    private GetCatalogItemsResult _catalogItemsResult;
    public GetCatalogItemsResult CatalogItemsResult => _catalogItemsResult;

    private LoginResult _userPlayfabData;

    private string _testNameUser = "userCards";




    public string jsonCardsData = "";

    //хранилище данных. Все переменные вынести в нужное место
    public Dictionary<string, string> dicGameData = new Dictionary<string, string>()
    {
        //{"cards_full_save", ""},
        {"keys", "0"},
        {"gold_keys", "0"},
        {"labs", "0"},
    };


    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

    }


    private void Start()
    {
        LoginOrAutorizationAtPlayfab(_testNameUser);
    }


    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SetUserData();

        // SetUserData(DicCurrency);
    }

    private void OnApplicationQuit()
    {
       // SetUserData();
    }


    private void LoginOrAutorizationAtPlayfab(string userId)
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = _titleGameId;
        }

        var request = new LoginWithCustomIDRequest { CustomId = userId, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }


    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        _userPlayfabData = result;
        //RefreshCatalog();
        //RefreshPlayerInventory();


        //SceneManager.LoadScene("SampleScene");
        LoadGame().Forget();
    }

    private async UniTask LoadGame()
    {

        List<string> listKeys = new List<string>();
        listKeys.Add(CardsData.FULL_SAVE_KEY);
        listKeys.Add(GameData.KEY_SAVE_GAME_DATA);

        var allData = await GetUserDataAsync(listKeys);

        Debug.Log(allData.Count);
        foreach (var item in allData)
        {
            Debug.Log($"{item.Key}    {item.Value}");
        }

        //Тут вернулось 2 индекса

        if(allData.ContainsKey(CardsData.FULL_SAVE_KEY))
            jsonCardsData = allData[CardsData.FULL_SAVE_KEY];


        if(allData.ContainsKey(GameData.KEY_SAVE_GAME_DATA))
        {
            var loadedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(allData[GameData.KEY_SAVE_GAME_DATA]);
            foreach (var item in loadedData)
            {
                if (dicGameData.ContainsKey(item.Key))
                {
                    dicGameData[item.Key] = item.Value; // Обновляем существующий ключ
                }
                else
                {
                    dicGameData.Add(item.Key, item.Value); // Добавляем новый ключ (если нужно)
                }
            }
        }
      

        SceneManager.LoadScene("SampleScene");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void RefreshCatalog()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(),
            result => {

                _catalogItemsResult = result;



                //var shopModule = Instantiate(_prefabShopModule, transform);
                //shopModule.Initialize(this);

                // Обработка успешного получения каталога
                //Debug.Log("Каталог получен. Найдено предметов: " + result.Catalog.Count);
                //foreach (var item in result.Catalog)
                //{


                //    Debug.Log($"Предмет: {item.DisplayName}, ID: {item.ItemId}, VirtualCurrencyPrices: {item.VirtualCurrencyPrices.Count}, " +
                //        $"{item.ItemClass}" +
                //        $"   {item.Tags.Count}");
                //    // Здесь вы можете сохранить item в свой список или отобразить в UI
                //}
            },
            error => {
                // Обработка ошибки
                Debug.LogError($"Ошибка получения каталога: {error.GenerateErrorReport()}");
            });
    }

    public void RefreshPlayerInventory()
    {


        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => {
                // Обработка успешного получения инвентаря
                Debug.Log("Инвентарь игрока получен");

                _userInventory = result;


                //var energyModule = Instantiate(_prefabEnergyModule, transform);
                //energyModule.Initialize(this);


                // Выводим баланс всех валют
                //foreach (var currency in result.VirtualCurrency)
                //{
                //    // currency.Key - название валюты (например, "GD" для Gold)
                //    // currency.Value - текущий баланс
                //    Debug.Log($"Валюта {currency.Key}: {currency.Value}");

                //    // Пример: если у вас есть валюта "GM" (Gems), можно обновить UI
                //    if (currency.Key == "GM")
                //    {
                //        Debug.Log($"Обновление {currency.Key}: {currency.Value}");
                //        //UpdateGemsUI(currency.Value);
                //    }

                //    if (currency.Key == "EN")
                //    {
                //        Debug.Log($"Обновление {currency.Key}: {currency.Value}");
                //        //UpdateGemsUI(currency.Value);
                //    }
                //}

                // Здесь также можно обработать список предметов result.Inventory
                foreach (var item in result.Inventory)
                {
                    Debug.Log($"У игрока есть предмет: {item.DisplayName}, количество: {item.RemainingUses ?? 1}, {item.ItemInstanceId}");
                }
            },
            error => {
                Debug.LogError($"Ошибка получения инвентаря: {error.GenerateErrorReport()}");
            });
    }





    //Сохранения

    //Записать данные
    public void SetUserData(string key, string json)
    {
        Debug.Log("SetUserData");

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {key, json}
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void SetUserData()
    {
        Debug.Log("SetUserData");

        //сохранить все данные GameData в одну строку
        string gameDataJson = JsonConvert.SerializeObject(dicGameData);


        //Debug.Log("SetUserData");

        //data["cards_full_save"] = CardsData.GetDataCardsToString(); //Актуализируем информацию о карточках

        Dictionary<string,string> allSaveData = new Dictionary<string,string>();
        allSaveData.Add(CardsData.FULL_SAVE_KEY, CardsData.GetDataCardsToString());
        allSaveData.Add(GameData.KEY_SAVE_GAME_DATA, gameDataJson);


        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = allSaveData
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }


    public async UniTask SetUserDataAsync(string key, string json)
    {
        var tcs = new UniTaskCompletionSource<bool>();

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            { key, json }
        }
        },
        result => {
            Debug.Log("Successfully updated user data");
            tcs.TrySetResult(true);
        },
        error => {
            Debug.Log("Got error setting user data");
            Debug.Log(error.GenerateErrorReport());
            tcs.TrySetException(new Exception(error.GenerateErrorReport()));
        });

        await tcs.Task;
    }





    //Получить данные по конкретному ключу
    public string GetUserData(string key)
    {
        string playFabId = _userPlayfabData.PlayFabId;
        string res = null;



        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            //PlayFabId = playFabId,
            Keys = new List<string> { key }

        }, result => {
            Debug.Log($"Got user data: {result.Data}");
            if (result.Data == null)
            {
                Debug.Log("Not Data");
                
            }

            else
            {
                foreach (var item in result.Data)
                {
                    Debug.Log($"{item.Key} ----   {item.Value.Value}");
                }

                UserDataRecord record = new UserDataRecord();
                if (result.Data.TryGetValue(key, out record))
                {
                    res = record.Value;
                }




                    //result.Data.TryGetValue(key);

                    //res = result.Data[key].Value;
                Debug.Log($"HAS DATA {res}");
            }

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });

        return res;
    }

    //Получить данные по конкретному ключу АСИНХРОННО
    public async UniTask<string> GetUserDataAsync(string key)
    {
        var tcs = new UniTaskCompletionSource<string>();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = new List<string> { key }
        }, result => {
            if (result.Data == null || !result.Data.ContainsKey(key))
            {
                tcs.TrySetResult(null);
            }
            else
            {
                tcs.TrySetResult(result.Data[key].Value);
            }
        }, error => {
            tcs.TrySetException(new Exception(error.GenerateErrorReport()));
        });

        return await tcs.Task;
    }

    //Получить данные по ключам Асинхронно
    public async UniTask<Dictionary<string, string>> GetUserDataAsync(List<string> keys)
    {
        var tcs = new UniTaskCompletionSource<Dictionary<string, string>>();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = keys
        }, result => {
            Debug.Log("Got user data:");

            if (result.Data == null)
            {
                Debug.Log("No Data");
                tcs.TrySetResult(new Dictionary<string, string>());
            }
            else
            {
                var dataDict = new Dictionary<string, string>();

                foreach (var item in result.Data)
                {
                    Debug.Log($"{item.Key} ---- {item.Value.Value}");
                    dataDict[item.Key] = item.Value.Value;
                }

                tcs.TrySetResult(dataDict);
            }
        }, error => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
            tcs.TrySetException(new Exception(error.GenerateErrorReport()));
        });

        return await tcs.Task;
    }


    //Получить данные по ключам
    public void GetUserData(List<string> keys)
    {
        string playFabId = _userPlayfabData.PlayFabId;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            //PlayFabId = playFabId,
            Keys = keys
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null)
                Debug.Log("Not Data");
            else
            {
                foreach (var item in result.Data)
                {
                    Debug.Log($"{item.Key} ----   {item.Value.Value}");
                }
            }

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }


    //Получить все данные
    public void GetUserData()
    {
        string playFabId = _userPlayfabData.PlayFabId;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            //PlayFabId = playFabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null)
                Debug.Log("Not Data");
            else
            {
                foreach (var item in result.Data)
                {
                    Debug.Log($"{item.Key} ----   {item.Value.Value}");
                }
            }

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }


    //Удалить данные
    public void RemoveUserData(string key)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            KeysToRemove = new List<string> { key }  // ← удаляем ключ
        },
        result => Debug.Log($"Key {key} removed"),
        error => Debug.LogError(error.GenerateErrorReport()));
    }

}
