import UIKit
import WebKit

class ViewController: UIViewController, WKNavigationDelegate, WKUIDelegate {

    private var webView: WKWebView!

    override var prefersStatusBarHidden: Bool {
        return true
    }

    override var prefersHomeIndicatorAutoHidden: Bool {
        return true
    }

    override var supportedInterfaceOrientations: UIInterfaceOrientationMask {
        return .portrait
    }

    override func viewDidLoad() {
        super.viewDidLoad()
        view.backgroundColor = UIColor(red: 1.0, green: 0.96, blue: 0.97, alpha: 1.0)
        setupWebView()
        loadGame()
    }

    private func setupWebView() {
        let config = WKWebViewConfiguration()
        config.allowsInlineMediaPlayback = true
        config.mediaTypesRequiringUserActionForPlayback = []

        let preferences = WKWebpagePreferences()
        preferences.allowsContentJavaScript = true
        config.defaultWebpagePreferences = preferences

        // Allow reading local file assets in sandbox
        config.setValue(true, forKey: "allowUniversalAccessFromFileURLs")

        webView = WKWebView(frame: view.bounds, configuration: config)
        webView.autoresizingMask = [.flexibleWidth, .flexibleHeight]
        webView.navigationDelegate = self
        webView.uiDelegate = self
        webView.isOpaque = false
        webView.backgroundColor = .clear

        // Disable rubber-banding bounce for full-screen game experience
        webView.scrollView.bounces = false
        webView.scrollView.contentInsetAdjustmentBehavior = .never
        webView.scrollView.showsVerticalScrollIndicator = false
        webView.scrollView.showsHorizontalScrollIndicator = false

        view.addSubview(webView)
    }

    private func loadGame() {
        // Priority 1: Check WebApp/PlayableStudio/index.html folder reference
        if let htmlUrl = Bundle.main.url(forResource: "index", withExtension: "html", subdirectory: "WebApp/PlayableStudio") {
            let readAccessUrl = htmlUrl.deletingLastPathComponent().deletingLastPathComponent() // Base WebApp dir
            webView.loadFileURL(htmlUrl, allowingReadAccessTo: readAccessUrl)
            return
        }

        // Priority 2: Direct bundle resource search
        if let htmlPath = Bundle.main.path(forResource: "index", ofType: "html") {
            let htmlUrl = URL(fileURLWithPath: htmlPath)
            webView.loadFileURL(htmlUrl, allowingReadAccessTo: Bundle.main.bundleURL)
            return
        }

        print("Error: Could not locate PlayableStudio/index.html in main bundle!")
    }

    // MARK: - WKNavigationDelegate
    func webView(_ webView: WKWebView, didFinish navigation: WKNavigation!) {
        print("Kids Nail Salon loaded successfully!")
    }

    func webView(_ webView: WKWebView, didFail navigation: WKNavigation!, withError error: Error) {
        print("Web load failed: \(error.localizedDescription)")
    }
}

