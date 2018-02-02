using System.Security;
using System.Windows;

namespace WeighBridgeApplication.Util
{
    public static class PasswordBoxMvvmAttachedProperties
    {
        public static readonly DependencyProperty EncryptedPasswordProperty =
            DependencyProperty.RegisterAttached("EncryptedPassword", typeof (SecureString),
                typeof (PasswordBoxMvvmAttachedProperties));

        public static SecureString GetEncryptedPassword(DependencyObject obj)
        {
            return (SecureString) obj.GetValue(EncryptedPasswordProperty);
        }

        public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for EncryptedPassword.  This enables animation, styling, binding, etc...
    }
}