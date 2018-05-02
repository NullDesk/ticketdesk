## Back End

### Setup

To run a local development copy of the project, the startup project needs to be set to ngWebClientAPI, with its specified port being 50067.

### Configuration

The API supports configuration via the[ Web.config file located under the ngWebClientAPI project](https://github.com/kixiQu/ticketdesk/blob/ngWebClient-Dev/TicketDesk/ngWebClientAPI/Web.config) for ticket types, priority levels, and ticket categories.  More settings can be added by adding a new section under "ConfigSections", then adding data under the new section specified in "ConfigSections".

### Security Settings

Windows Authentication needs to be enabled. Anonymous Authentication needs to be disabled. 

```
<authentication mode="Windows"/>
    <authorization>
    	<deny users="?"/>
	</authorization>
```

Located in the Web.config file

## Front End

### Setup

Running the front end requires installing [Node.js](https://nodejs.org/en/) first (which will include Node Package Manager aka NPM), then [Angular 4](https://angular.io/), [ng-bootstrap](https://ng-bootstrap.github.io/#/home), and [TypeScript](https://www.typescriptlang.org/).

In order to do this, you may need to configure the npm proxy. This can be done by running:

```
npm config set proxy http://clarkpud%2F<username>:<password>@pudebcs:8080
npm config set https-proxy http://clarkpud%2F<username>:<password>@pudebcs:8080
```

Run `npm install` in [the ngWebClient-Standalone directory](https://github.com/kixiQu/ticketdesk/tree/ngWebClient-Dev/TicketDesk/ngWebClient-StandAlone) to install dependencies according to what's specified in [package.json](https://github.com/kixiQu/ticketdesk/blob/ngWebClient-Dev/TicketDesk/ngWebClient-StandAlone/package.json).

Run` npm install -g @angular/cli` to install Angular globally.

Once the dependencies are installed, you can run the application front the command line using: `ng serve` to run on the default port of 4200, or use: `ng serve --port PORT_NUMBER` to choose a different port.

### Configuration

To make web service calls to the backend, you must set the `serviceBaseURL` constant in `app-settings.ts` to the base URL where the endpoint can be reached.
