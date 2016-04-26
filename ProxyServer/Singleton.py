#region -----------------Info-----------------
#Name: Singleton
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from threading import *
#endregion -----------------Imports-----------------

#region -----------------Class-----------------

class Singleton(type):
    # A metaclass to all of the singleton classes that makes them be singletons.
    _instances = {}
    _lock = Lock()

    def __call__(cls, *args, **kwargs):
        while True:
            acquired = cls._lock.acquire()
            if acquired:
                if cls not in cls._instances:
                    cls._instances[cls] = super(Singleton, cls).__call__(*args, **kwargs)
                cls._lock.release()
                return cls._instances[cls]


#endregion -----------------Class-----------------