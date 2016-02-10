#region -----------------Info-----------------
#Name: Printer
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
import time
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class Printer(object):
    # An object that is used to print messages to the textual user interface.
    __metaclass__ = Singleton

    def __init__(self):
        print 'Current time:%s' % (self.__getFullTime())

    def printMessage(self, class_name, message):
        # Prints a message.
        print '%s-%s: %s' % (class_name, self.__getTime(), message)

    def __getTime(self):
        # Returns the time in this format: Hour:Minute:Second.
        return time.strftime("%H:%M:%S", time.strptime(time.ctime()))

    def __getFullTime(self):
        # Returns the time in this format: Day(of the week) Month Day(of the month) Hour:Minute:Second.
        return time.strftime("%a %b %d %H:%M:%S %Y", time.strptime(time.ctime()))


#endregion -----------------Class-----------------