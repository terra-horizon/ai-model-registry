from dependency_injector import containers, providers

from app.core.config import get_oidc_settings
from app.security.token_validator import TokenValidator
from app.services.photo_service import PhotoService


class Container(containers.DeclarativeContainer):
    wiring_config = containers.WiringConfiguration(
        modules=[
            "app.api.photos",
            "app.security.current_user",
        ])

    config = providers.Configuration()
    token_validator = providers.Singleton(TokenValidator, settings=providers.Object(get_oidc_settings()), )
    photo_service = providers.Factory(PhotoService, )
