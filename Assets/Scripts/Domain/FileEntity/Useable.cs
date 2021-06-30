using Data;
using Scripts;

public interface Useable
{
    bool Use(PlayerData target);
    bool CanUse();
}
