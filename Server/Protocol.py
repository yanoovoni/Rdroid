#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
from Printer import *
from Settings import *
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Protocol(object):
    __metaclass__ = Singleton
    printer = Printer()
    settings = Settings()

    def encodeLogin(self):


#endregion -----------------Class-----------------