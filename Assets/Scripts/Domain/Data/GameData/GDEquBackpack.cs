using Domain.Data.FileData;
using Scripts.Commons.Utils;

namespace Domain.Data.GameData
{
    public class GDEquBackpack:GDEquipment
    {
        private FDEquBackpack _fdEquBackpack;

        private GDBase[] _containData;

        public GDBase[] ContainData
        {
            get => _containData;
            set => _containData = value;
        }
        /// <summary>
        /// 背包可以使用
        /// </summary>
        /// <returns></returns>
        public override bool CanUse()
        {
            return true;
        }
        /// <summary>
        /// 使用背包（装备）
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool Use(GDBase target)
        {
            return true;
        }
        /// <summary>
        /// 背包不能使用其他物品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool CanReceiveUse(GDBase data)
        {
            return false;
        }


        public bool AddContainData(GDBase gdBase)
        {
            for (var i = 0; i < _containData.Length; i++)
            {
                if (_containData[i] == null)
                {
                    _containData[i] = gdBase;
                    return true;
                }
            }

            return false;
        }
        public GDEquBackpack Assemble(long id)
        {
            FDEquBackpack fdEqu = _fdService.GetFD(id) as FDEquBackpack;
            if (fdEqu != null)
            {
                _fdEquBackpack = ItemUtil.Clone(fdEqu);
                _icon = _resourceService.LoadSpriteByIO(fdEqu.icon);
                _containData = new GDBase[_fdEquBackpack.capacity];
                _title = _fdEquBackpack.title;
                ID = fdEqu.id;          
            }
            return this;
        }
    }
}