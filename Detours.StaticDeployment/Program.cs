using Detours.StaticDeployment;

var connectionString = @"";

await new Deployer()
	.WithUsers("users.json")
	.WithTours("tours.json")
	.WithReviews("reviews.json")
	.WithConnectionString(connectionString)
	.RunAsync(default);
