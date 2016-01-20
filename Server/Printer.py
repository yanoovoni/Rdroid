#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
import time
from Singleton import *
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Printer(object):
    __metaclass__ = Singleton

    def __init__(self):
        print 'Current time:%s' % (self.__getFullTime())

    def __getTime(self):
        return time.strftime("%H:%M:%S", time.strptime(time.ctime()))

    def __getFullTime(self):
        return time.strftime("%a %b %d %H:%M:%S %Y", time.strptime(time.ctime()))

    def printMessage(self, class_name, message):
        print '%s-%s: %s' % (class_name, self.__getTime(), message)


#endregion -----------------Class-----------------