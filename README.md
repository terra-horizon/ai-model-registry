# AI Model Registry

TODO Description

## Development

Download the repository and create a virtual environment. Execute

```bash
pip freeze > requirements.txt
```

to create the dependencies file and then install the dependencies running

```bash
pip install -r requirements.txt
```

Afterward, create a .env file inside the project root that holds the following values:
```dotenv
APP_NAME='AI Model Registry'
APP_VERSION=1.0.0
DEBUG=true

OIDC_ISSUER=
OIDC_AUDIENCE=
OIDC_CLIENT_ID=
OIDC_CLIENT_SECRET=
```
Fill missing fields accordingly. Finally, run the app using

```bash
uvicorn main:app --reload
```

To deploy the app, instead use

```dotenv
DEBUG=false
```

to mute development settings, like detailed exception stacks.