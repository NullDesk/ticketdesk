
JSFILES=jquery-wmd-plugin.js wmd.js showdown.js

all: jquery.wmd.js jquery.wmd.min.js

jquery.wmd.js: $(JSFILES)
	cat $(JSFILES) > $@

jquery.wmd.min.js: $(JSFILES)
	cat $(JSFILES) | python jsmin.py > $@
