﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProjectBaseEntities : DbContext
    {
        public ProjectBaseEntities()
            : base("name=ProjectBaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bar_to_weak> bar_to_weak { get; set; }
        public virtual DbSet<barrier> barrier { get; set; }
        public virtual DbSet<def_to_objtype> def_to_objtype { get; set; }
        public virtual DbSet<defenseobj> defenseobj { get; set; }
        public virtual DbSet<defenseobj_types> defenseobj_types { get; set; }
        public virtual DbSet<indust_object> indust_object { get; set; }
        public virtual DbSet<@object> @object { get; set; }
        public virtual DbSet<prot_to_bar> prot_to_bar { get; set; }
        public virtual DbSet<prot_to_obj> prot_to_obj { get; set; }
        public virtual DbSet<protect_mech> protect_mech { get; set; }
        public virtual DbSet<war_to_prot> war_to_prot { get; set; }
        public virtual DbSet<warning_type> warning_type { get; set; }
        public virtual DbSet<warnings> warnings { get; set; }
        public virtual DbSet<weak_to_object> weak_to_object { get; set; }
        public virtual DbSet<weakness> weakness { get; set; }
    }
}
