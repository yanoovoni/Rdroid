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

#region -----------------Class-----------------


class IDGenerator(object):
    # An object that generates different ids for the connected sessions.
    __metaclass__ = Singleton
    __last_id = 0 # Saves the last id that was used.

    def generateId(self):
        # Generates and returns a new id by taking the number of the last id and increasing it by 1.
        if self.outOfIds():
            self.resetIds()
        self.__last_id += 1
        return str(self.__last_id)

    def resetIds(self):
        # Sets __last_id to 0.
        self.__last_id = 0

    def outOfIds(self):
        # Returns whether this object is out of ids (which is very unlikely).
        if self.__last_id == 2147483647:
            return True
        return False


#endregion -----------------Class-----------------