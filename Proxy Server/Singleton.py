#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------

#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Singleton(type):
    _instances = {}

    def __call__(cls, *args, **kwargs):
        if cls not in cls._instances:
            cls._instances[cls] = super(Singleton, cls).__call__(*args, **kwargs)
        return cls._instances[cls]


#endregion -----------------Class-----------------