namespace GitSeeker;

public class Configuration
{
    public class Service
    {
        public string Name = "Unnamed";

        public class RepositoryInfo
        {
            public Dictionary<string, object> Connection;
            public string Branch = "master";
            public string Focus = "/";
        }

        public RepositoryInfo Repository;

        public class ActionInfo
        {
            public string Name = "Unnamed";
            public string Action;
            public Dictionary<string, object?> Parameters;
        }

        public List<ActionInfo> Actions;
    }

    public List<Service> Services;

    public static Configuration Sample = new()
    {
        Services = new ()
        {
            new ()
            {
                Name = "Sample Service",
                Repository = new ()
                {
                    Connection = new ()
                    {
                        {"type", "local"},
                        {"path", "/some/your/repo.git"}
                    },
                },
                Actions = new ()
                {
                    new ()
                    {
                        Name = "Clone New Version",
                        Action = "clone",
                        Parameters = new ()
                        {
                            {"path", "/home/build/sample"}
                        }
                    },
                    new ()
                    {
                        Name = "Build",
                        Action = "cmd",
                        Parameters = new ()
                        {
                            {"cmd", new []{"cd /home/build/sample", "sudo docker build -t sample ."}}
                        }
                    }
                }
            }
        }
    };
}

