﻿using System.Text;
using Newtonsoft.Json;

namespace GitSeeker;

public static class Seeker
{
    public const int SEEK_DELAY_MS = 5000;
    
    public static void Loop(Configuration config)
    {
        Console.WriteLine($"Seeker Started");
        
        Dictionary<string, RepoState> state = new Dictionary<string, RepoState>();
        if (File.Exists("state.json"))
            try
            {
                var s = JsonConvert.DeserializeObject<Dictionary<string, RepoState>>(File.ReadAllText("state.json"));
                if(s != null)
                    state = s;
            }
            catch (Exception e)
            {
                // ignored
            }

        IGitProvider[] providers = new IGitProvider[config.Services.Count];
        string[] hashes = new string[config.Services.Count];
        for (int i = 0; i < config.Services.Count; i++)
        {
            StringBuilder conn = new ();
            foreach (var kvp in config.Services[i].Repository.Connection)
                conn.Append($"{kvp.Key}-{kvp.Value}");
            hashes[i] = RepoState.CalcRepoHash(config.Services[i].Name, conn.ToString(), config.Services[i].Repository.Branch);
            
            //TODO: Multiple Providers
            providers[i] = new LocalGitProvider((string)config.Services[i].Repository.Connection["path"]);
            
            if (!state.ContainsKey(hashes[i]))
                state.Add(hashes[i], new RepoState("", new ()));
        }
        
        File.WriteAllText("state.json", JsonConvert.SerializeObject(state));
        
        Console.WriteLine($"State Initialized");
        
        while (true)
        {
            //1.Get Repo Hashes
            //2.1. Compare To State
            //2.2. Do Some Actions
            //3.Update State
            
            for (int i = 0; i < config.Services.Count; i++)
            {
                //GET NEW STATE
                var service = config.Services[i];
                var provider = providers[i];
                var hash = hashes[i];
                var branch = provider.GetBranches().FirstOrDefault(x=>x.Name==service.Repository.Branch);
                if(branch == null)
                    continue;
                if (branch!.LastCommit != state[hash].LastCommit)
                {
                    //New Commit
                    state[hash].LastCommit = branch.LastCommit;
                    var commit = provider.GetCommit(branch.LastCommit);
                    Console.WriteLine($"Got new commit on {service.Name} at {service.Repository.Branch}");
                    var tree = provider.GetTreeRecursive(commit.Tree);
                    foreach (var file in tree)
                    {
                        if(!file.path.RepresentFocusDir(service.Repository.Focus))
                            continue;
                        
                        Console.WriteLine($"Changed file in focus ({service.Repository.Focus}): {file.path}");
                        Console.WriteLine("Starting actions...");
                        //Actions
                        Dictionary<string, object?> variables = new ();
                        foreach (var action in service.Actions)
                        {
                            Console.WriteLine($"Running [{action.Name}]");
                            ActionRunner.StartAction(action.Action, action.Parameters, provider, service, variables);
                        }
                        
                        break;
                    }

                    state[hash].TreeHashes = new ();
                    foreach (var file in tree)
                    {
                        if(!file.path.RepresentFocusDir(service.Repository.Focus))
                            continue;
                        state[hash].TreeHashes.Add(file.path, file.hash);
                    }
                }
            }
            File.WriteAllText("state.json", JsonConvert.SerializeObject(state));
            Thread.Sleep(SEEK_DELAY_MS);
        }
    }
}