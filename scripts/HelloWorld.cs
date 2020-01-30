using Godot;
using System;

using System.IO;

using NHibernate.Cfg;
using NHibernate;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

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

            session.Save(Product);
            session.Flush();

            //session.Close();
            }
            catch (Exception e)
            {
                log.Error("Fehler bei der codeausführung.", e);
            }
    }

    ISession initHibernate()
    {
        FileInfo i = new FileInfo(".");
        log.Info(i.FullName);

        // read hibernate.cfg.xml, initializing hibernate
        var cfg = new Configuration();
        cfg.Configure();
        //cfg.AddAssembly(i.FullName + System.IO.Path.DirectorySeparatorChar + "mappings\\Price.hbm.xml");
        cfg.AddAssembly(typeof(Product).Assembly);
        cfg.AddAssembly(typeof(Price).Assembly);

        // Get ourselves an NHibernate Session
        var sessions = cfg.BuildSessionFactory();
        return sessions.OpenSession();
    }
}

public class Product
{
    public string Name { get; set; }
    public int Category { get; set; }
    public bool Discontinued { get; set; }
    public Price Price { get; set; }
}

public class Price
{
    public int min { get; set; }
    public int max { get; set; }
    public int average { get; set; }
}
