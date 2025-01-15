using Irc;
using irc_backend.Services;

User user;

user = new User();
user.Token = "irWeytewfOdMatAmtyepjushvoufrokicAtHa";
user.Nick = "0000";
IrcServiceServer.usersByToken.Add(user.Token, user);

user = new User();
user.Nick = "0001";
user.Token = "acgijQuaphDoibGapailOihajakivItFokjiodbi";
IrcServiceServer.usersByToken.Add(user.Token, user);


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<IrcServiceServer>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
// app.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true)
app.Run();
