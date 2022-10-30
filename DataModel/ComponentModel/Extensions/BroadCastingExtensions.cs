namespace DataModel.ComponentModel
{
    public static class BroadcastingExtensions
    {
        public static void Broadcast(this INotifyPropertyChanged sender, string propertyName)
        {
            sender.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        public static void Broadcast(this INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            sender.PropertyChanged?.Invoke(sender, args);
        }
    }
}
