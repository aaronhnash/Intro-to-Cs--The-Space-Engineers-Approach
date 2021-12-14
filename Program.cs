using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace intro_to_cs
{
    
    class Collector{
        private string name;
        private int weight;
        private int level;
        private int cost;

        private bool active = false;
        public Collector(string name){
            this.name = name;
            this.weight = 0;
            this.level = 1;
            this.cost = 0;
            if (this.name == "basic"){
                this.weight = 1;
                this.cost = 16;
            }
            else if (this.name == "enhanced"){
                this.weight = 8;
                this.cost = 64;
            }
            else if (this.name == "advanced"){
                this.weight = 32;
                this.cost = 256;
            }
            else if (this.name == "perfected"){
                this.weight = 128;
                this.cost = 1024;
            }
            
        }
        public int get_production(){
            int val = 0;
            val = (this.weight)*(this.level);
            return val;
        }
        
        public int get_level_cost(){
            return ((this.cost)*(this.level)*2);
        }

        public void level_up(ref int money){
            if (money >= this.get_level_cost() ){
                this.level +=1;
            }
        
        }
    
        public int get_level(){
            return this.level;
        }
    
    }
    

    class Counter {


        public double total; // Total number of "bugs" caught
        // The collectors that are active. (number, weight)

        private List<Collector> collectors_1 = new List<Collector>();
        private List<Collector> collectors_2 = new List<Collector>();
        private List<Collector> collectors_3 = new List<Collector>();
        private List<Collector> collectors_4 = new List<Collector>();
        private int curr_count;
        private int basic_cost = 16;
        private int enhanced_cost = 64;
        private int advanced_cost = 256;
        private int perfected_cost = 1024;

        public Counter(){
            this.curr_count = 0;
        }
        public int get_total_production(){
            int production = 0;
            foreach (Collector item in collectors_1){
                production += item.get_production();
            }
            foreach (Collector item in collectors_2){
                production += item.get_production();
            }
            foreach (Collector item in collectors_3){
                production += item.get_production();
            }
            foreach (Collector item in collectors_4){
                production += item.get_production();
            }
            return production;
        }
        public void tick(){
            this.curr_count += this.get_total_production();
            
        }
        public void curr_count_inc(){
            this.curr_count += 1;
        }
        public void disp_curr_count(){
            Console.WriteLine("Current bug count: {0}", this.curr_count);
        }
        public void display_collectors(){
            Console.WriteLine($"Basic Count: {collectors_1.Count}");
            Console.WriteLine($"Enhanced Count: {collectors_2.Count}");
            Console.WriteLine($"Advanced Count: {collectors_3.Count}");
            Console.WriteLine($"Perfected Count: {collectors_4.Count}");
        }
        private List<Collector> get_collector(string name){
            if (name.ToLower() == "basic"){
                return collectors_1;
            }
            else if (name.ToLower() == "enhanced"){
                return collectors_2;
            }
            else if (name.ToLower() == "advanced"){
                return collectors_3;
            }
            else if (name.ToLower() == "perfected"){
                return collectors_4;
            }
            else {
                Console.WriteLine("No collector found by that name {0}",name);
                return null;
            }
        }
        public void display_collector(string name){
            List<Collector> target = this.get_collector(name);
            if (target is not null){
                Console.WriteLine("Found {0}x '{1}' collectors", target.Count, name);
                for (int i=0; i < target.Count; i++){
                    Console.WriteLine($"Collector {i} lvl: {target[i].get_level()}, production: {target[i].get_production()}, lvl cost: {target[i].get_level_cost()}");
                }
            }
        }
        public void level_collector(string name, int index){
            List<Collector> target = this.get_collector(name);
            target[index].level_up(ref this.curr_count);
        }
        public void add_collector(string name){
            switch (name){
                case "basic":
                    if (this.curr_count >= basic_cost){
                        this.curr_count -= basic_cost;
                        collectors_1.Add(new Collector("basic"));
                    }
                    else {Console.WriteLine("Insufficient funds, need {0} to purchase basic collector.", basic_cost);}
                    break;
                case "enhanced":
                    if (this.curr_count >= enhanced_cost){
                        this.curr_count -= enhanced_cost;
                        collectors_2.Add(new Collector("enhanced"));
                    }
                    else {Console.WriteLine("Insufficient funds, need {0} to purchase enhanced collector.", enhanced_cost);}
                    break;
                case "advanced":
                    if (this.curr_count >= advanced_cost){
                        this.curr_count -= advanced_cost;
                        collectors_3.Add(new Collector("advanced"));
                    }
                    else {Console.WriteLine("Insufficient funds, need {0} to purchase advanced collector.", advanced_cost);}
                    break;
                case "perfected":
                    if (this.curr_count >= perfected_cost){
                        this.curr_count -= perfected_cost;
                        collectors_4.Add(new Collector("perfected"));
                    }
                    else {Console.WriteLine("Insufficient funds, need {0} to purchase perfected collector.", perfected_cost);}
                    break;
            }
        }
}

    public class Program
    {
        static Counter main_counter = new Counter();
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            Task.Run(() => AsyncUpdate());
            while (true){
                //Collect user input here, spits into input commands
                Console.WriteLine("Awaiting input...");
                string[] input = (Console.ReadLine()).Split();
                var cmd = input[0];
                switch (cmd)
                {
                    case "info":
                        // If there's a specific query, then display only one collector.
                        try {main_counter.display_collector(input[1]);}
                        // Otherwise, display all of them.
                        catch {main_counter.display_collectors();}
                        main_counter.disp_curr_count();
                        break;
                    case "click":
                        main_counter.curr_count_inc();
                        break;
                    case "upgrade":
                        try {main_counter.level_collector(input[1], int.Parse(input[2]));}
                        catch {Console.WriteLine("Invalid input.");}
                        break;
                    case "help":
                        Console.WriteLine("Available commands:\ninfo (collector type)[OPTIONAL]\nupgrade (collector type) (collector index)\nclick\nbuy (collector type)");
                        break;
                    case "buy":
                        try {main_counter.add_collector(input[1]);}
                        catch {Console.WriteLine("Invalid input.");}
                        break;
                }
                Console.WriteLine("\n");
            }
        }
        static void AsyncUpdate(){
            while (true){
                Thread.Sleep(1000);
                // Any update related code will go here!
                Program.main_counter.tick();

                }
        }
    }
}
