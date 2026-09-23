/**
* Template Name: Mentor
* Template URL: https://bootstrapmade.com/mentor-free-education-bootstrap-theme/
* Updated: Aug 07 2024 with Bootstrap v5.3.3
* Author: BootstrapMade.com
* License: https://bootstrapmade.com/license/
*/

(function () {
    "use strict";

    /**
     * Apply .scrolled class to the body as the page is scrolled down
     */
    function toggleScrolled() {
        const selectBody = document.querySelector('body');
        const selectHeader = document.querySelector('#header');
        if (!selectHeader) return;
        if (!selectHeader.classList.contains('scroll-up-sticky') && !selectHeader.classList.contains('sticky-top') && !selectHeader.classList.contains('fixed-top')) return;
        window.scrollY > 100 ? selectBody.classList.add('scrolled') : selectBody.classList.remove('scrolled');
    }

    document.addEventListener('scroll', toggleScrolled);
    window.addEventListener('load', toggleScrolled);

    /**
     * Scroll top button
     */
    let scrollTop = document.querySelector('.scroll-top');

    function toggleScrollTop() {
        if (scrollTop) {
            window.scrollY > 100 ? scrollTop.classList.add('active') : scrollTop.classList.remove('active');
        }
    }

    if (scrollTop) {
        scrollTop.addEventListener('click', (e) => {
            e.preventDefault();
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }

    window.addEventListener('load', toggleScrollTop);
    document.addEventListener('scroll', toggleScrollTop);

    /**
     * Animation on scroll function and init
     */
    function aosInit() {
        if (typeof AOS !== 'undefined') {
            AOS.init({
                duration: 600,
                easing: 'ease-in-out',
                once: true,
                mirror: false
            });
        }
    }
    window.addEventListener('load', aosInit);

    /**
     * Initiate glightbox
     */
    if (typeof GLightbox !== 'undefined') {
        const glightbox = GLightbox({
            selector: '.glightbox'
        });
    }

    /**
     * Initiate Pure Counter
     */
    if (typeof PureCounter !== 'undefined') {
        new PureCounter();
    }

    /**
     * Init swiper sliders
     */
    function initSwiper() {
        if (typeof Swiper === 'undefined') return;
        document.querySelectorAll(".init-swiper").forEach(function (swiperElement) {
            let configElement = swiperElement.querySelector(".swiper-config");
            if (!configElement) return;

            let config = JSON.parse(configElement.innerHTML.trim());

            if (swiperElement.classList.contains("swiper-tab")) {
                if (typeof initSwiperWithCustomPagination === 'function') {
                    initSwiperWithCustomPagination(swiperElement, config);
                }
            } else {
                new Swiper(swiperElement, config);
            }
        });
    }

    window.addEventListener("load", initSwiper);

})();


/* ==========================================
   HEADER MOBILE RESPONSIVE & DROPDOWN LOGIC
========================================== */
document.addEventListener("DOMContentLoaded", function () {

    const toggle = document.querySelector(".mobile-nav-toggle");
    const navmenu = document.querySelector("#navmenu");

    if (!toggle || !navmenu) return;

    /* TOGGLE MAIN MENU */
    toggle.addEventListener("click", function (e) {
        e.preventDefault();
        e.stopPropagation();

        document.body.classList.toggle("mobile-nav-active");
        const opened = document.body.classList.contains("mobile-nav-active");

        toggle.setAttribute("aria-expanded", opened ? "true" : "false");
        const icon = toggle.querySelector("i");
        if (icon) {
            icon.classList.toggle("bi-list", !opened);
            icon.classList.toggle("bi-x", opened);
        }
    });

    /* NAVMENU CLICK DELEGATION */
    navmenu.addEventListener("click", function (e) {

        // Dropdown Toggle Link Par Click
        const dropdownLink = e.target.closest(".dropdown-toggle");

        if (dropdownLink) {
            if (window.innerWidth <= 1199) {
                e.preventDefault();
                e.stopPropagation();

                const dropdown = dropdownLink.parentElement;

                // Doosre Khule hue Dropdowns Close karein
                navmenu.querySelectorAll(".dropdown.dropdown-active").forEach(function (item) {
                    if (item !== dropdown) {
                        item.classList.remove("dropdown-active");
                    }
                });

                // Toggle Current Dropdown
                dropdown.classList.toggle("dropdown-active");
            }
            return; // Drodown handling finish, closeMobileMenu run nahi hoga
        }

        // Regular Link Click Par Menu Close karein
        const normalLink = e.target.closest("a");
        if (normalLink && window.innerWidth <= 1199) {
            closeMobileMenu();
        }

    });

    /* CLICK OUTSIDE TO CLOSE */
    document.addEventListener("click", function (e) {
        if (!navmenu.contains(e.target) && !toggle.contains(e.target)) {
            closeMobileMenu();
        }
    });

    /* WINDOW RESIZE */
    window.addEventListener("resize", function () {
        if (window.innerWidth >= 1200) {
            closeMobileMenu();
        }
    });

    /* CLOSE FUNCTION */
    function closeMobileMenu() {
        document.body.classList.remove("mobile-nav-active");
        if (toggle) {
            toggle.setAttribute("aria-expanded", "false");
            const icon = toggle.querySelector("i");
            if (icon) {
                icon.classList.remove("bi-x");
                icon.classList.add("bi-list");
            }
        }

        if (navmenu) {
            navmenu.querySelectorAll(".dropdown-active").forEach(function (dropdown) {
                dropdown.classList.remove("dropdown-active");
            });
        }
    }

});