#! /usr/bin/env python
#  Copyright Joel de Guzman 2002-2007. Distributed under the Boost
#  Software License, Version 1.0. (See accompanying file LICENSE_1_0.txt
#  or copy at http://www.boost.org/LICENSE_1_0.txt)
#  Hello World Example from the tutorial

import HelloBoostPython

# you have to change the extension of the DLL to .pyd or otherwise Python will not be able to load it.
# https://stackoverflow.com/questions/11036319/boost-python-hello-world-example-not-working-in-python

print(HelloBoostPython.greet())
