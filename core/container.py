from dependency_injector import containers, providers

from core.config import get_oidc_settings
from security.token_validator import TokenValidator
from services.photo_service import PhotoService


class Container(containers.DeclarativeContainer):
    wiring_config = containers.WiringConfiguration(
        modules=[
            "api.photos",
            "security.current_user",
        ])

    config = providers.Configuration()
    token_validator = providers.Singleton(TokenValidator, settings=providers.Object(get_oidc_settings()), )
    photo_service = providers.Factory(PhotoService, )
