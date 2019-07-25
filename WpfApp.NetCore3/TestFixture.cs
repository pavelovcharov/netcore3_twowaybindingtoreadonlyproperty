using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

[assembly: NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]

namespace Test {
    class ObjectWithReadonlyProperty : INotifyPropertyChanged {
        string readonlyProperty;
        public ObjectWithReadonlyProperty(string v) {
            readonlyProperty = v;
        }
        public string ReadonlyProperty { get { return readonlyProperty; } private set { readonlyProperty = value; } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged() {
            PropertyChanged(this, new PropertyChangedEventArgs(null)); //-V3083
        }
    }
    [TestFixture]
    public class TestFixture {
        [Test]
        public void TwoWayBindingToReadonlyProperty() {
            var btn = new Button();
            var data = new ObjectWithReadonlyProperty("12345");
            BindingOperations.SetBinding(btn, Button.ContentProperty, new Binding(nameof(ObjectWithReadonlyProperty.ReadonlyProperty)) { Mode = BindingMode.TwoWay, Source = data });
            Assert.AreEqual("12345", btn.Content);
            btn.Content = "67890";
            Assert.AreEqual("12345", data.ReadonlyProperty);
        }
    }
}
