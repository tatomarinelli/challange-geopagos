
# Challenge Payments - Geopagos - 2024

El proyecto consiste en dos microservicios, ClientAuthorization (El publico hacia el cliente) y PaymentProccesor (Servicio interno simulando procesador de pago)

Se sumó un proyecto más llamado "ServiceMiddlewares", sirve como Wrapper para la respuesta de los servicios y de esa forma "normalizarlos". Cuando la respuesta es OK el Wrapper llena un objeto "data" con la respuesta del servicio, cuando la respuesta es un error (Sin importar si la excepción es manejada/no manejada) el wrapper llena el objeto "Errors". De esta forma si existieran mas servicios se define una manera común en la que los mismos se comunican, podríamos tener otro Middleware o algún filtro que lea esta respuesta y tome decisiones en base a que datos vinieron (Sabiendo que siempre se componen igual).

El CRON que valida la confirmación de las operaciones se encuentra en el proyecto de ClientAuthorization. Tiene un diccionario con las operaciones pendientes y se encarga de llamar a la BL una vez pasado ese tiempo.
## Endpoints desarrollados -  ClientAuthorization

#### Payment (Entrypoint de todo el flujo)

```http
  POST /v1/Client/Authorization/Payment
```

| Campo | Tipo     | Descripcion                |
| :-------- | :------- | :------------------------- |
| `clientType` | `string` | **Required**. "1" o "2" según el modelo de autorización|
| `transactionData.amount` | `decimal` | **Required**. Monto de la operacion|

#### Confirm (Confirmacion de la operacion)

```http
  POST /v1/Client/Authorization/Confirm
```

| Campo | Tipo     | Descripcion                |
| :-------- | :------- | :------------------------- |
| `` | `string` | **Required**. Id de la operacion en la BD. Va directamente el string (TODO: Agregar campo ID) |

## Endpoints desarrollados -  PaymentProcessor

#### Payment (Autoriza según si es decimal o entero)

```http
  POST /v1/PaymentProcessor/Payment
```

| Campo | Tipo     | Descripcion                |
| :-------- | :------- | :------------------------- |
| `clientType` | `string` | **Required**. "1" o "2" según el modelo de autorización|
| `transactionData.amount` | `decimal` | **Required**. Monto de la operacion|

## Consultas
#### Operacion de tipo usuario 1 - Autorizada
```json
{
  "clientID": "string",
  "clientType": "1",
  "cardData": {
    "holderName": "string",
    "type": "string",
    "number": "string",
    "cvc": "string",
    "expirationDate": "2024-02-09T07:17:29.492Z"
  },
  "transactionData": {
    "amount": 150.00,
    "date": "2024-02-09T07:17:29.492Z"
  }
}
```
#### Operacion de tipo usuario 2 - Pendiente de confirmacion
```json
{
  "clientID": "string",
  "clientType": "2",
  "cardData": {
    "holderName": "string",
    "type": "string",
    "number": "string",
    "cvc": "string",
    "expirationDate": "2024-02-09T07:17:29.492Z"
  },
  "transactionData": {
    "amount": 150,
    "date": "2024-02-09T07:17:29.492Z"
  }
}
```
#### Operacion de tipo usuario 2 - Denegada
```json
{
  "clientID": "string",
  "clientType": "2",
  "cardData": {
    "holderName": "string",
    "type": "string",
    "number": "string",
    "cvc": "string",
    "expirationDate": "2024-02-09T07:17:29.492Z"
  },
  "transactionData": {
    "amount": 150.50,
    "date": "2024-02-09T07:17:29.492Z"
  }
}
```



## Base de Datos

Para la base de datos se utilizó Postgres. Se puede conectar utilizando localhost:5432 (O el puerto elegido en el config.env)

Usuario: postgres - Contraseña: postgres - Source: geopagos_db

Consiste de dos tablas, una de logueo histórico y otra intermedia para las operaciones pendientes de confirmación (Esta tabla se mantiene sincronizada con el Hosted Service que elimina las operaciones cada X cantidad de tiempo).

No realicé un volumen, una vez que se mata el contenedor los datos se pierden (Decisión mia para no tener que andar borrando datos en cada prueba)

## XUnit

Para correr los tests utilicé Visual Studio, intenté de todas formas (Ya obsesionado) para poder correrlos en el contenedor y si bien lo logré tuve varios problemas para poder comunicarme con el servicio de Paymentprocessor (Luego de miles de intentos supongo que el problema era que estaba intentando correrlo en el build y debería crear un script para que se haga una vez que los servicios estén levantados).

Levantar el proyecto utilizando docker-compose (no el del visual studio) y luego correr los tests normalmente (Esto va a hacer que los tests corran de forma local y la conexión al servicio de paymentprocessor se haga al contenedor)... no me convence.

## Que falta?

- Mejores validaciones las request no se están validando, podemos enviar cualquier tipo de cliente o valores negativos.
- Crear excepciones personalizadas (Aprovechar el wrapper para poder informar bien los errores).
- Añadir más tests.
- Arreglar el logueo de las fechas (En las tablas quedó como DateOnly, hay que cambiar y hacer nuevamente el Scaffold)
- Mejorar el chequeo de tiempo en el CRON, revisar mejor de no estar teniendo RaceConditions con la BD.
- Parametrizar el tiempo del CRON (Quedó en 45 segundos)
- Cambiar la PK de un id autogenerado desde el 0 por un GUID 
- Refactorizar el código, limpiar warnings de null, limpiar casteos innecesarios entre numeros y strings.
- LOGGER, no se inyectó por lo que fui comentando los logueos, horrible.
- Constructores vacios.
- Consistencia en el estilo del código (Variables publicas con mayuscula, privadas con minuscula, readonly e interfaces con guionbajo y minuscula).
- Coleccion de Postman :D 
- Mejorar las envs y como se comportan los tests (Hay cosas hardcodeadas que cambian en un lugar y no se encuentran mas).
## Como levantarlo?

#### Cargar imagen

```bash
$ docker load < challenge_geopagos_docker.tar.gz
```

Podemos validarlo:

```bash
$ docker images
REPOSITORY            TAG       IMAGE ID       CREATED        SIZE
clientauthorization   dev       582b1f3cb6c7   44 hours ago   208MB
paymentprocessor      dev       dbe82d6fc98e   44 hours ago   208MB
postgres              15        e012816a6967   5 weeks ago    419MB
```

#### Correr docker-compose:
```bash
$ docker-compose up
... o si se editó el "config.env"
$ docker-compose up --build
```
... comandos útiles:
```bash
$ docker-compose down -v
$ docker stop $(docker ps -a -q)
$ docker remove $(docker ps -a -q)
```
(Si el volumen de la base de datos da error <pasa cuando se baja y sube varias veces> utilizar esos comandos, en ese orden para que el volumen se apague correctamente).
## Listo!

## URLs 
- clientauthorization: http://localhost:1000/swagger/index.html
- paymentprocessor: http://localhost:1001/swagger/index.html
#
