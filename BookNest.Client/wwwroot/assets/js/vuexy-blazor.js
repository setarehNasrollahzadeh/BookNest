/*
 * =============================================================================
 * Vuexy and Blazor Menu Integration
 * =============================================================================
 *
 * Why is this code needed?
 * Vuexy already provides sidebar toggle and hover functionality. However,
 * when integrated with Blazor, its JavaScript behavior may not work correctly
 * because Blazor can update or replace HTML elements during rendering.
 *
 * This code helps Vuexy work correctly with Blazor and adds the specific
 * sidebar behavior required by this application.
 *
 * Main responsibilities:
 * 1. Reinitialize the Vuexy menu when necessary.
 * 2. Handle sidebar toggle clicks correctly.
 * 3. Expand a collapsed sidebar on desktop hover and collapse it again
 *    when the mouse leaves, if it was opened temporarily by hover.
 * 4. Prevent the logo link from navigating to index.html.
 * 5. Prevent duplicate event listeners.
 * 6. Clear temporary hover behavior on mobile screens.
 *
 * Note:
 * This code depends on Vuexy's Menu, Helpers, and CSS classes.
 * The desktop breakpoint is set to 1200 pixels.
 */


window.vuexyBlazor = {
    _toggleHandlerInstalled: false,
    _hoverHandlersInstalled: false,
    _hoverCloseTimer: null,
    _isHoverOpening: false,

    reinitializeMenu: function () {
        const currentMenu = document.querySelector('#layout-menu');

        if (
            !currentMenu ||
            typeof window.Menu !== 'function' ||
            !window.Helpers
        ) {
            console.warn('Vuexy menu initialization skipped.');
            return;
        }

        // Reinitialize Vuexy menu for the current Blazor DOM.
        const oldMenu = window.Helpers.mainMenu;

        if (oldMenu && typeof oldMenu.destroy === 'function') {
            oldMenu.destroy();
        }

        const isHorizontal =
            currentMenu.classList.contains('menu-horizontal');

        const newMenu = new window.Menu(currentMenu, {
            orientation: isHorizontal ? 'horizontal' : 'vertical',
            closeChildren: isHorizontal,
            showDropdownOnHover: true
        });

        window.Helpers.mainMenu = newMenu;

        // Handle sidebar toggle buttons.
        if (!this._toggleHandlerInstalled) {
            document.addEventListener(
                'click',
                function (event) {
                    const target = event.target;

                    if (!(target instanceof Element)) {
                        return;
                    }

                    const toggle = target.closest('.layout-menu-toggle');

                    if (!toggle) {
                        return;
                    }

                    event.preventDefault();
                    event.stopImmediatePropagation();

                    const html = document.documentElement;

                    // A click makes the user's choice persistent.
                    window.vuexyBlazor._isHoverOpening = false;

                    if (window.vuexyBlazor._hoverCloseTimer) {
                        clearTimeout(
                            window.vuexyBlazor._hoverCloseTimer
                        );

                        window.vuexyBlazor._hoverCloseTimer = null;
                    }

                    // Remove temporary hover state before toggling.
                    html.classList.remove('layout-menu-hover');

                    window.Helpers.toggleCollapsed();
                },
                true
            );

            this._toggleHandlerInstalled = true;
        }

        // Desktop hover behavior for the entire sidebar.
        if (!this._hoverHandlersInstalled) {
            const isDesktop = function () {
                return window.innerWidth >= 1200;
            };

            const getMenu = function () {
                return document.querySelector('#layout-menu');
            };

            const isCollapsed = function () {
                return document.documentElement.classList.contains(
                    'layout-menu-collapsed'
                );
            };

            const clearHoverTimer = function () {
                if (window.vuexyBlazor._hoverCloseTimer) {
                    clearTimeout(
                        window.vuexyBlazor._hoverCloseTimer
                    );

                    window.vuexyBlazor._hoverCloseTimer = null;
                }
            };

            const openTemporaryHover = function () {
                if (!isDesktop() || !isCollapsed()) {
                    return;
                }

                const html = document.documentElement;

                // Mark this as a temporary opening caused by hover.
                window.vuexyBlazor._isHoverOpening = true;
                html.classList.add('layout-menu-hover');
            };

            const closeTemporaryHover = function () {
                clearHoverTimer();

                window.vuexyBlazor._hoverCloseTimer = setTimeout(
                    function () {
                        if (!isDesktop()) {
                            return;
                        }

                        const menu = getMenu();

                        // Do not close while the pointer is still
                        // somewhere inside the sidebar.
                        if (menu && menu.matches(':hover')) {
                            return;
                        }

                        const html = document.documentElement;

                        if (
                            window.vuexyBlazor._isHoverOpening &&
                            isCollapsed()
                        ) {
                            html.classList.remove('layout-menu-hover');
                        }

                        window.vuexyBlazor._isHoverOpening = false;
                        window.vuexyBlazor._hoverCloseTimer = null;
                    },
                    100
                );
            };

            // Entering any part of the sidebar opens it temporarily.
            document.addEventListener(
                'mouseover',
                function (event) {
                    if (!isDesktop()) {
                        return;
                    }

                    const menu = getMenu();
                    const target = event.target;

                    if (
                        !menu ||
                        !(target instanceof Node) ||
                        !menu.contains(target)
                    ) {
                        return;
                    }

                    // Ignore movement between elements inside the menu.
                    if (
                        event.relatedTarget instanceof Node &&
                        menu.contains(event.relatedTarget)
                    ) {
                        return;
                    }

                    clearHoverTimer();
                    openTemporaryHover();
                }
            );

            // Leaving the entire sidebar closes only a temporary opening.
            document.addEventListener(
                'mouseout',
                function (event) {
                    if (!isDesktop()) {
                        return;
                    }

                    const menu = getMenu();
                    const target = event.target;

                    if (
                        !menu ||
                        !(target instanceof Node) ||
                        !menu.contains(target)
                    ) {
                        return;
                    }

                    // Ignore movement between elements inside the menu.
                    if (
                        event.relatedTarget instanceof Node &&
                        menu.contains(event.relatedTarget)
                    ) {
                        return;
                    }

                    closeTemporaryHover();
                }
            );

            // Prevent the brand link from navigating to index.html.
            document.addEventListener(
                'click',
                function (event) {
                    const target = event.target;

                    if (!(target instanceof Element)) {
                        return;
                    }

                    const brand = target.closest(
                        '#layout-menu .app-brand-link'
                    );

                    if (brand) {
                        event.preventDefault();
                    }
                },
                true
            );

            // Clear temporary hover state when switching to mobile.
            window.addEventListener('resize', function () {
                if (isDesktop()) {
                    return;
                }

                clearHoverTimer();

                document.documentElement.classList.remove(
                    'layout-menu-hover'
                );

                window.vuexyBlazor._isHoverOpening = false;
            });

            this._hoverHandlersInstalled = true;
        }

        console.info('Vuexy menu initialized for the current Blazor DOM.');
    }
};
