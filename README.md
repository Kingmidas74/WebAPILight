# Web API Light

There is a my custom template for .Net Core WebAPI solution. I want to create something that I could use for quick start.

## Acknowledgments

Well... I want to thank all of this people. I started this project just like an example of Web API, but step-by-step I improve and extend architecture after new info.

* [nevoroman](https://github.com/nevoroman) with [video from DotNext2019](https://www.youtube.com/watch?v=9s_4wpzENhg)
* [marshinov](https://habr.com/ru/users/marshinov/) with [DotNext2018](https://www.youtube.com/watch?v=qJPwSvDLmQE)
* [AnatolyKulakov](https://github.com/AnatolyKulakov) with [Structure ogging](https://www.youtube.com/watch?v=wy9YbBqhHqQ) and [metrics](https://www.youtube.com/watch?v=AFB89L8DLpE) (Yep, I chose prometheus instead of AppMetrics. Doesn't matter)
* [jasontaylordev](https://github.com/jasontaylordev) with great video about [clean architecture](https://www.youtube.com/watch?v=dK4Yb6-LxAk) and [testing](https://www.youtube.com/watch?v=2UJ7mAtFuio)

## Attention
```diff
If you believe that this project has infringed any copyrights in any way or just don't want to be mention, please contact me with email kingmidas1992@gmail.com.
```

## Components

* .WebAPI services - Incoming point
    * [.Net Core WebAPI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio) - Basic framework
    * [Swagger](https://swagger.io/) - It's a very simple in use and informative instrument for documentation your API
* Business services - Separate class library with example of some "business" logic. In this case I implement next techniques:
    * [MediatR](https://github.com/jbogard/MediatR) - for CQRS 
    * [Specification](https://www.c-sharpcorner.com/article/the-specification-pattern-in-c-sharp/) - for Q in CQRS
    * [FluentValidation](https://fluentvalidation.net/) - basically.. for validation.
* DataAccess
    * [Ef Core](https://github.com/dotnet/efcore) - ORM
    * [Npgsql](https://www.npgsql.org/) - additional package for support PostgreSQL
* NotificationWorker - example of .Net Core Worker template in action. There is only recieving message from bus and log it.
* [RabbitMQ](https://www.rabbitmq.com/) as bus for connect API and Worker
* [Seq](https://datalust.co/seq) - support structure logging with [Serilog](https://serilog.net/)
* [Prometheus](https://prometheus.io/) and [Grafana](https://grafana.com/) for analytics
* And some tests of course

## Installation

1) Just clone repository ...
```bash
git clone https://github.com/Kingmidas74/WebAPILight.git
```
2) check settings in docker-compose.yml

3) ... and run docker-compose from solution folder.
```bash
docker-compose up -d
```
That's all!

## Usage

Services will be available on the next adresses by default.

| Service       |       URI     |
| ------------- | ------------- |
| Web API (Angular client)  | http://localhost:5002  |
| Identity Service  | http://localhost:5000  |
| RabbitMQ  | http://localhost:15672  |
| Seq  | http://localhost:5340  |
| prometheus  | http://localhost:9090  |
| grafana  | http://localhost:3000  |


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)