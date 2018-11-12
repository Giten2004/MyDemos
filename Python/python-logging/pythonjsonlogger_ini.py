import logging
import logging.config
import pythonjsonlogger

logging.config.fileConfig('pythonjsonlogger.ini')
logger = logging.getLogger()


logger.debug('often makes a very good meal of %s', 'visiting tourists')
