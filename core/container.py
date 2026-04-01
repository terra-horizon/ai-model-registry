from dependency_injector import containers, providers

from services.photo_service import PhotoService


class Container(containers.DeclarativeContainer):
    wiring_config = containers.WiringConfiguration(
        modules=[
            "api.photos",
        ])

    config = providers.Configuration()

    photo_service = providers.Factory(PhotoService, )
