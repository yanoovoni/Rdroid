#region -----------------Info-----------------
#Name: Singleton
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Class-----------------


class Singleton(type):
    # A metaclass to all of the singleton classes that makes them be singletons.
    _instances = {}

    def __call__(cls, *args, **kwargs):
        if cls not in cls._instances:
            cls._instances[cls] = super(Singleton, cls).__call__(*args, **kwargs)
        return cls._instances[cls]


#endregion -----------------Class-----------------