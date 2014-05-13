<script type="text/javascript">
        $(function () {

            tab_id = "iframeBody";
            loading_id = "divLoading";
            _InitFrame();
            function _InitFrame() {
                //Phan xu ly loading
                obj = $("#" + tab_id)[0];
                try {
                    obj.attachEvent('onload', function () { _fnframeLoad(tab_id) });
                } catch (e) {
                }
                obj.onload = function () {
                    _fnframeLoad(tab_id)
                }


                //
                $curFrame = getCurFrame();
                try {
                    obj = $(frames[tab_id].window)[0];
                } catch (e) { }

                CallBeforeUnload(obj, tab_id);

                $divLoadFrame = $curFrame.prev();
                //Khống chế lúc đang loading tab 1, thì chọn tab 2 , trở lại tab 1 vẫn còn loading        
                try {
                    iframe = $curFrame[0];
                    iframe = iframe.contentWindow || iframe.contentDocument;
                    if (iframe.document) iframe = iframe.document;
                }
                catch (e) {
                    iframe = $(frames[$curFrame.attr("id")].document)[0];
                }
                //     

                var _timer;
                _timer = setInterval(function () { //lặp tới khi trang load hoàn tất       

                    try {
                        if (iframe.readyState == 'complete') {
                            if ($divLoadFrame.is(":visible")) {
                                isFrameBusy = false;
                                $divLoadFrame.hide(); // Download is complete 
                                clearInterval(_timer);
                                clearTimeout(_timer);
                            }
                            clearInterval(_timer);
                            clearTimeout(_timer);
                        }
                    } catch (e) {
                        clearInterval(_timer);
                        clearTimeout(_timer);

                    }   //end if      
                }, 1)
            }

            /**************** Loading ****************/

            function getCurFrame() {
                return $("#" + tab_id);
            }


            function geCurLoading() {
                return $("#" + loading_id);
            }

            function _fnframeLoad(tab_id) {

                var theframe;
                try {
                    obj = $(frames[tab_id].window)[0];
                }
                catch (e) { }

                /*** Hide loading ***/
                $curFrame = getCurFrame();
                $divLoadFrame = $curFrame.prev();
                $divLoadFrame.hide();
                CallBeforeUnload(obj, tab_id);

            }


            function CallBeforeUnload(obj, tab_id) {

                try {
                    obj.attachEvent('onbeforeunload', function () { _fnframeUnLoad(tab_id) });
                } catch (e) { }
                obj.onbeforeunload = function () {
                    _fnframeUnLoad(tab_id);
                }
            }

            function _fnframeUnLoad(tab_id) {

                $curFrame = getCurFrame();
                $divLoadFrame = $curFrame.prev();

                if ($divLoadFrame) {
                    isFrameBusy = true;
                    $divLoadFrame.show();
                }

                window.onbeforeunload = null;
                window.onunload = null;
            }


            /**************** End Loading ****************/
        });                    //end $(function ()
       </script>