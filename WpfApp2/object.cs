//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp2
{
    using System;
    using System.Collections.ObjectModel;
    
    [Serializable]public partial class @object
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public @object()
        {
            this.prot_to_obj = new ObservableCollection<prot_to_obj>();
            this.weak_to_object = new ObservableCollection<weak_to_object>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string desciption { get; set; }
        public Nullable<int> critical_importance { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<prot_to_obj> prot_to_obj { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<weak_to_object> weak_to_object { get; set; }
    }
}
