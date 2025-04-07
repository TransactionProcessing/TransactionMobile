namespace TransactionMobile.Maui.BusinessLogic.UIServices
{
    public interface IDeviceService
    {
        String GetIdentifier();

        String GetModel();

        String GetPlatform();

        Boolean IsIOS();

        String GetManufacturer();

        void SetOrientation(Orientation displayOrientation);
    }

    public enum Orientation {
        Unknown = 0,
        Portrait = 1,
        Landscape = 2
    }
}
