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


class IDGenerator(object):
    __metaclass__ = Singleton
    __current_id = 0

    def generateId(self):
        if self.outOfIds():
            self.resetIds()
        self.__current_id += 1
        return str(self.__current_id)

    def resetIds(self):
        self.__current_id = 0

    def outOfIds(self):
        if self.__current_id == 2147483647:
            return True
        return False


#endregion -----------------Class-----------------