import httpx
import jwt
from jwt import PyJWKClient

from app.core.config import OidcSettings


class TokenValidator:
    def __init__(self, settings: OidcSettings):
        self._settings = settings
        self._openid_config = self._load_openid_config()
        self._jwks_client = PyJWKClient(self._openid_config["jwks_uri"])

    def _load_openid_config(self) -> dict:
        discovery_url = (
            f"{self._settings.issuer.rstrip('/')}"
            "/.well-known/openid-configuration"
        )
        response = httpx.get(discovery_url, timeout=10.0)
        response.raise_for_status()
        return response.json()

    def validate_access_token(self, token: str) -> dict:
        signing_key = self._jwks_client.get_signing_key_from_jwt(token)
        payload = jwt.decode(
            token,
            signing_key.key,
            algorithms=["RS256"],
            audience=self._settings.audience,
            issuer=self._settings.issuer,
            options={
                "require": ["exp", "iat"],
                "verify_signature": True,
                "verify_aud": True,
                "verify_iss": True,
            },
        )

        return payload
