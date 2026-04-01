from functools import lru_cache

from pydantic import Field
from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    app_name: str = Field(default="My FastAPI API", alias="APP_NAME")
    app_version: str = Field(default="1.0.0", alias="APP_VERSION")
    app_env: str = Field(default="development", alias="APP_ENV")
    debug: bool = Field(default=False, alias="DEBUG")

    model_config = SettingsConfigDict(env_file=".env", env_file_encoding="utf-8", case_sensitive=False, extra="ignore", )


class OidcSettings(BaseSettings):
    issuer: str = Field(default="", alias="OIDC_ISSUER")
    audience: str = Field(default="", alias="OIDC_AUDIENCE")
    client_id: str = Field(default="", alias="OIDC_CLIENT_ID")
    client_secret: str | None = Field(default=None, alias="OIDC_CLIENT_SECRET")

    model_config = SettingsConfigDict(env_file=".env", env_file_encoding="utf-8", case_sensitive=False, extra="ignore", )


@lru_cache
def get_settings() -> Settings:
    return Settings()


@lru_cache
def get_oidc_settings() -> OidcSettings:
    return OidcSettings()
