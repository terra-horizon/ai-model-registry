from typing import Annotated

import jwt
from dependency_injector.wiring import inject, Provide
from fastapi import HTTPException, Security, status, Depends
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer

from core.container import Container
from security.models import CurrentUser
from security.token_validator import TokenValidator

bearer_scheme = HTTPBearer(auto_error=True)


@inject
async def get_current_user(
        credentials: HTTPAuthorizationCredentials = Security(bearer_scheme),
        token_validator: Annotated[TokenValidator, Depends(Provide[Container.token_validator]),] = None,
) -> CurrentUser:
    if credentials.scheme.lower() != "bearer":
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid authentication scheme.", )

    token = credentials.credentials

    try:
        payload = token_validator.validate_access_token(token)
    except jwt.ExpiredSignatureError:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Token has expired.", )
    except jwt.InvalidAudienceError:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid token audience.", )
    except jwt.InvalidIssuerError:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid token issuer.", )
    except jwt.InvalidTokenError:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid access token.", )

    return CurrentUser(
        sub=payload["sub"],
        username=payload.get("preferred_username") or payload.get("username"),
        email=payload.get("email"),
        access_token=token,
    )
