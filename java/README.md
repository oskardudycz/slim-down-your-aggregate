[![Twitter Follow](https://img.shields.io/twitter/follow/oskar_at_net?style=social)](https://twitter.com/oskar_at_net) [![Github Sponsors](https://img.shields.io/static/v1?label=Sponsor&message=%E2%9D%A4&logo=GitHub&link=https://github.com/sponsors/oskardudycz/)](https://github.com/sponsors/oskardudycz/) [![blog](https://img.shields.io/badge/blog-event--driven.io-brightgreen)](https://event-driven.io/?utm_source=cqrs_is_simpler_jvm) [![blog](https://img.shields.io/badge/%F0%9F%9A%80-Architecture%20Weekly-important)](https://www.architecture-weekly.com/?utm_source=cqrs_is_simpler_jvm) 

![Github Actions](https://github.com/oskardudycz/cqrs-is-simpler-with-java/actions/workflows/samples_event-sourcing-esdb-simple.yml/badge.svg?branch=main) 

# CQRS is simpler than you may think

You can also watch the .NET version of talk [below](https://www.youtube.com/watch?v=iY7LO289qnQ):

<a href="https://www.youtube.com/watch?v=iY7LO289qnQ" target="_blank"><img src="https://img.youtube.com/vi/iY7LO289qnQ/0.jpg" alt="CQRS is Simpler than you think with C#11 & NET7" width="640" height="480" border="10" /></a>

Repository with backing code for my talk. For more samples like that see [EventSourcing in Java repository](https://github.com/oskardudycz/EventSourcing.JVM).

## Read also
-   📝 [CQRS facts and myths explained](https://event-driven.io/en/cqrs_facts_and_myths_explained/?utm_source=event_sourcing_net)
-   📝 [What onion has to do with Clean Code?](https://event-driven.io/pl/onion_clean_code/?utm_source=event_sourcing_net)
-   📝 [How to slice the codebase effectively?](https://event-driven.io/en/how_to_slice_the_codebase_effectively/?utm_source=event_sourcing_net)
-   📝 [Generic does not mean Simple?](https://event-driven.io/en/generic_does_not_mean_simple/?utm_source=event_sourcing_net)
-   📝 [Can command return a value?](https://event-driven.io/en/can_command_return_a_value/?utm_source=event_sourcing_net)
-   📝 [How to register all CQRS handlers by convention](https://event-driven.io/en/how_to_register_all_mediatr_handlers_by_convention/?utm_source=event_sourcing_net)

## Prerequisites

1. Install git - https://git-scm.com/downloads.
2. Install Java JDK 17 (or later) - https://www.oracle.com/java/technologies/downloads/.
3. Install IntelliJ, Eclipse, VSCode or other preferred IDE.
4. Install docker - https://docs.docker.com/engine/install/.
5. Open project folder.

## Running

1. Run: `docker-compose up`.
2. Wait until all dockers got are downloaded and running.
3. You should automatically get:
    - PG Admin - IDE for postgres. Available at: http://localhost:5050.
        - Login: `admin@pgadmin.org`, Password: `admin`
        - To connect to server Use host: `postgres`, user: `postgres`, password: `Password12!`
4. Open, build and run `ECommerceApplication`.
    - Swagger should be available at: http://localhost:8080/swagger-ui/index.html

See also [.NET version of those samples](https://github.com/oskardudycz/cqrs-is-simpler-with-net-and-csharp).
