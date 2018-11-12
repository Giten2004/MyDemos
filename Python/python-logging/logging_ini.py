import logging
import logging.config

logging.config.fileConfig('logging.ini')
logger = logging.getLogger()
logger.debug('often makes a very good meal of %s', 'visiting tourists')
