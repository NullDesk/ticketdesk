WMD: The Wysiwym Markdown Editor
================================

Introduction
------------

This is the branch of wmd for [Open Library](http://openlibrary.org), forked from the [Stackoverflow branch of wmd](http://github.com/derobins/wmd).

Major Changes
-------------

* Explicit function call is required to enable WMD editor for a textarea
* Can work with multiple textareas in the same page
* jquery plugin to simplify the usage

How to use
----------

Example using jquery plugin:

    <html>
        <head>
            <title>WMD Example using jquery</title>
            <link rel="stylesheet" type="text/css" href="wmd.css"/>

            <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
            <script type="text/javascript" src="jquery.wmd.min.js"></script>
        </head>
        <body>
            <h1>WMD Example using jquery</h1>
            <div>
                <textarea id="notes"/>
            </div>
    
            <script type="text/javascript">
                $().ready(function() {
                   $("#notes").wmd(); 
                });
            </script>
        </body>
    </html>
    
Example without using jquery:

    <html>
        <head>
            <title>WMD Example</title>
            
            <link rel="stylesheet" type="text/css" href="wmd.css"/>
            
            <script type="text/javascript" src="wmd.js"></script>
            <script type="text/javascript" src="showdown.js"></script>
        </head>
        <body>
            <h1>WMD Example</h1>

            <div>
                <div id="notes-button-bar"/>
                <textarea id="notes"/>
                <div id="notes-preview"/>
            </div>
    
            <script type="text/javascript">
                setup_wmd({
                    "input": "notes",
                    "button_bar": "notes-button-bar",
                    "preview": "notes-preview",
                });
            </script>
        </body>
    </html>

License
-------

WMD Editor is licensed under [MIT License](http://github.com/openlibrary/wmd/raw/master/License.txt).


