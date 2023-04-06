namespace Lantern.Platform;

public interface ISingleProcessInstanceManager
{
    void ActivateOtherProcessMainWindow();
    bool Lock();
}
