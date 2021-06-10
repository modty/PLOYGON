namespace Scripts
{
    public interface IAttribute
    {
        /// <summary>
        /// 攻击UI显示的字符串
        /// </summary>
        /// <returns></returns>
        string BaseValue();
        string CurrentValue();
    }
}