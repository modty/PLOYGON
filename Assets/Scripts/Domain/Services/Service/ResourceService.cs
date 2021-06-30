using System.IO;
using Loxodon.Framework.Services;
using Newtonsoft.Json;
using UnityEngine;

namespace Domain.Services.IService
{
    /// <summary>
    /// 资源加载服务，封装游戏中加载资源的各种方法。包括优化加载方式（阻塞、协程、线程）等方式。
    /// </summary>
    public class ResourceService:BaseService
    {
        public ResourceService(IServiceContainer container) : base(container)
        {
            
        }


        protected override void OnStart(IServiceContainer container)
        {
            
        }


        protected override void OnStop(IServiceContainer container)
        {
            
        }

        /// <summary>
        /// 使用 IO 流加载图片，并返回。
        /// </summary>
        /// <param name="_url">图片地址</param>
        /// <returns></returns>
        public Texture2D LoadTexture2DByIO(string _url)
        {
            //创建文件读取流
            FileStream _fileStream = new FileStream("Assets/Resources/"+_url, FileMode.Open, FileAccess.Read);
            _fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] _bytes = new byte[_fileStream.Length];
            _fileStream.Read(_bytes, 0, (int)_fileStream.Length);
            _fileStream.Close();
            _fileStream.Dispose();
            //创建Texture
            Texture2D _texture2D = new Texture2D(1, 1);
            _texture2D.LoadImage(_bytes);
            return _texture2D;
        }
        public Texture2D LoadTexture2DByResources(string _url)
        {
            return Resources.Load<Texture2D>(_url);
        }
        /// <summary>
        /// 使用 IO 流加载图片，并将图片转换成 Sprite 类型返回
        /// </summary>
        /// <param name="_url">图片地址</param>
        /// <returns></returns>
        public Sprite LoadSpriteByIO(string _url)
        {
            Texture2D _texture2D = null;
            if ("unknown".Equals(_url))
                _texture2D= LoadTexture2DByIO("Resources/Sprites/Default.png");
            else
                _texture2D= LoadTexture2DByIO(_url);
            Sprite _sprite = Sprite.Create(_texture2D, new Rect(0, 0, _texture2D.width, _texture2D.height), new Vector2(0.5f, 0.5f));
            return _sprite;
        }
        public Sprite LoadSpriteByResources(string _url)
        {
            return Resources.Load<Sprite>(_url);
        }
        /// <summary>
        /// 加载预制体
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public GameObject LoadPrefabByResources(string prefab)
        {
            return Resources.Load<GameObject>(prefab);
        }

        /// <summary>
        /// 读取JSON文本
        /// </summary>
        /// <param name="sync"></param>
        /// <param name="dir"></param>
        /// <typeparam name="T">实体对象</typeparam>
        /// <returns></returns>
        public T LoadJSON<T>(bool task,string dir)
        {
            StreamReader streamReader = new StreamReader("Assets/Resources/"+dir);
            string str = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}