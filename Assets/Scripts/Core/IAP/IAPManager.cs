using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
public class IAPManager : MonoBehaviour,IStoreListener
{
	  // Singleton instance
    public static IAPManager Instance { get; private set; }

    private IStoreController storeController;
    private List<string> productIds = new List<string> { "p01", "p02", "p03" };

    private void Awake()
    {
        // Kiểm tra và đảm bảo chỉ có một instance duy nhất
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ đối tượng này khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy nếu đã có instance khác
        }
    }

    async void Start()
    {
        // Khởi tạo Unity Gaming Services
        await InitializeUnityServices();

        InitializePurchasing();
    }

    private async Task InitializeUnityServices()
    {
        if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Gaming Services Initialized!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error initializing Unity Gaming Services: {e.Message}");
            }
        }
    }

    public void InitializePurchasing()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No Internet Connection! Can't fetch prices.");
            return;
        }

        if (IsInitialized()) return;

        var builder = ConfigurationBuilder.Instance(
	        StandardPurchasingModule.Instance()
        );
        // Thêm các sản phẩm vào IAP
        foreach (var productId in productIds)
        {
            builder.AddProduct(productId, ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized() => storeController != null;

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP Initialized Successfully!");
        storeController = controller;

        // Log thông tin các sản phẩm
        foreach (var product in storeController.products.all)
        {
            Debug.Log($"Product: {product.definition.id}, Price: {product.metadata.localizedPriceString}, Currency: {product.metadata.isoCurrencyCode}");
        }
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"IAP Initialization Failed: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"IAP Initialization Failed: {error}");
    }


    public void BuyProduct(string productId)
    {
        if (IsInitialized() && storeController.products.WithID(productId) != null)
        {
            storeController.InitiatePurchase(productId);
        }
        else
        {
            Debug.LogError($"Product {productId} is not available for purchase.");
        }
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args == null || args.purchasedProduct == null)
        {
            Debug.LogError("ProcessPurchase failed: args or purchasedProduct is null");
            return PurchaseProcessingResult.Pending;
        }
        string productId = args.purchasedProduct.definition.id;


        return PurchaseProcessingResult.Complete;
        //return PurchaseProcessingResult.Complete;
    }



    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase Failed: {product.definition.id}, Reason: {failureReason}");
    }

    // Phương thức bất đồng bộ trả về giá sản phẩm
    public async Task<string> GetLocalizedPriceAsync(string productId)
    {
        if (!IsInitialized())
        {
            Debug.Log("IAP not initialized yet. Waiting for initialization...");
            await WaitForInitialization();
        }

        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);
            if (product != null)
            {
                return $"{product.metadata.localizedPriceString} {product.metadata.isoCurrencyCode}";
            }
        }

        return "N/A";
    }

    private async Task WaitForInitialization()
    {
        while (!IsInitialized())
        {
            await Task.Yield();  // Đợi mỗi frame
        }
    }
}
