public interface IAmmoObserver
{
    void OnAmmoChanged(int currentAmmo, int reserveAmmo, bool isReloading);
}
