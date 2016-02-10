#region -----------------Info-----------------
#Name: Filter
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
from Singleton import *
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class Filter(object):
    # An object that is used for filtering the messages that do not fit the protocol.
    __metaclass__ = Singleton

    def filter(self, message):
        # Returns whether the given message fits the protocol or does not.
        return True


#endregion -----------------Class-----------------