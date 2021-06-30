using System;
using Domain.Services.IService;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Services;
using UnityEngine;
using UnityEngine.UI;

public class TestResource : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    private ApplicationContext _context;
    private IServiceContainer _container;

    private void Awake()
    {
        // 创建全局上下文
        _context = Context.GetApplicationContext();
        //获得上下文中的服务容器
        _container = _context.GetContainer();
    }

    /// <summary>
    /// 注册服务
    /// </summary>
    private void RegisterServices()
    {
        // 注册输入服务
        _container.Register<ResourceService>(new ResourceService(_container));
        _container.Register<PrefabService>(new PrefabService(_container));
            
    }
    private void Start()
    {
        RegisterServices();
        StartAllService();
    }

    private void StartAllService()
    {

        ResourceService resourceService = _container.Resolve<ResourceService>();
        resourceService.Start();
        
        PrefabService prefabService = _container.Resolve<PrefabService>();
        prefabService.Start();

        image.sprite = resourceService.LoadSpriteByResources("Textures/logo");
        image.sprite = resourceService.LoadSpriteByIO("Textures/logo.png");
     
        Instantiate(prefabService.Get("Prefabs/Scene/Characters/Characters_ModularParts_Static/Chr_ArmLowerLeft_Female_00_Static"));
        
        
        
    }
}
