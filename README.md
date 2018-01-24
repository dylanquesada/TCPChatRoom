# TCPChatRoom


Let's talk about the SOLID principles:
Single Responsibility Principle - This involves making sure that each
method only is responsible for performing one task. In the code below,
we have the SetName method with the sole task of assigning the name
variable for a client. This is a clear example of the Single Responsibility
principle in action.
 
 public void SetName(string message, Client client)
        {
            client.name = message.Substring(0, message.IndexOf(":"));
                        
        }

Dependency Inversion Principle - This principle offers that it is bad practice
to write tightly coupled code. Instead, the developer should aim to write code
where the lower level class is dependant on the higher level and any dependecies 
be injected to the lower level class in a manner that allows for easy manipulation
after the code is written. In the code below for example, we injected the logger into
the server through the constructor in order to leave the code closed for modification. 
(This is a third SOLID principle.) By injecting the dependency, the logger can be
swapped without affecting the remaining code.

Ilogger logger;
        public Dictionary<IMember, Client> users;
        Queue<string> messages;
        public Server(Ilogger logger)
        {
            ServerIP = "192.168.0.119";
            port = 9999;
            users = new Dictionary<IMember, Client>();
            messages = new Queue<string>();
            this.logger = logger;
            server = new TcpListener(IPAddress.Parse(ServerIP), port);
            server.Start();
            
        }