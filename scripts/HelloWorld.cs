using Godot;
using System;

using System.Collections.Generic;

using NHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "config/log4net.config")]

namespace TestProgram
{
	public class HelloWorld : RichTextLabel
	{
		//private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		log4net.ILog log;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			try
			{
				log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
				log.Info("Hallo erstmal.");
				ISession session = initHibernate();

				GD.Print("Hello World!!");
				this.Text = "Hello World";

				var Product = new Product
				{
					Name = "Produkt Name",
					Category = 1,
					Discontinued = false,

					Price = new Price
					{
						min = 0,
						max = 100,
						average = 50
					}
				};

				session.Save(Product.Price);
				session.Save(Product);
				session.Flush();

				}
				catch (Exception e)
				{
					log.Error("Fehler bei der codeausführung.", e);
				}
		}

		ISession initHibernate()
		{
			// read hibernate.cfg.xml, initializing hibernate
			var cfg = new Configuration();
			cfg.Configure("config/hibernate.cfg.xml");

			//cfg.AddAssembly(typeof(Product).Assembly);
			//cfg.AddAssembly(typeof(Price).Assembly);
			//cfg.AddFile("config/mappings/Price.hbm.xml");
			//cfg.AddFile("config/mappings/Product.hbm.xml");

			IEnumerable<string> Files = System.IO.Directory
				.EnumerateFiles("config/mappings", "*.*", System.IO.SearchOption.TopDirectoryOnly);
			
			foreach(string File in Files)
				if (File.EndsWith(".hbm.xml"))
					cfg.AddFile(File);

			// Create Tables in DB
			new SchemaExport(cfg).Create(false, true);

			// Get ourselves an NHibernate Session
			var sessions = cfg.BuildSessionFactory();
			return sessions.OpenSession();
		}
	}

	public class Product
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int Category { get; set; }
		public virtual bool Discontinued { get; set; }
		public virtual Price Price { get; set; }
		public virtual IList<Store> Stores { get; set; }
	}

	public class Price
	{
		public virtual int Id { get; set; }
		public virtual int min { get; set; }
		public virtual int max { get; set; }
		public virtual int average { get; set; }
	}

	public class Store
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IList<Product> Products { get; set; }
	}
}
