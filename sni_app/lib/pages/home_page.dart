import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import '../core/constants.dart';
import './partner_page.dart';
import './settings_page.dart';
import 'package:sni_app/l10n/app_localizations.dart';

// ===== HOME PAGE =====

class HomePage extends StatelessWidget {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context)!.appName),
        centerTitle: true,
      ),

      // ===== MENU HAMBURGER =====
      drawer: Drawer(
        child: ListView(
          padding: EdgeInsets.zero,
          children: [
            DrawerHeader(
              decoration: BoxDecoration(
                color: Theme.of(context).colorScheme.primary,
              ),
              child: const Text(
                AppConstants.appName,
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 22,
                ),
              ),
            ),
            ListTile(
              leading: const Icon(Icons.home),
              title: Text(AppLocalizations.of(context)!.home),
              onTap: () {
                Navigator.pop(context);
              },
            ),
            ListTile(
              leading: const Icon(Icons.people),
              title: Text(AppLocalizations.of(context)!.community),
              onTap: () {
                Navigator.pop(context);
              },
            ),
            ListTile(
              leading: const Icon(Icons.handshake),
              title: Text(AppLocalizations.of(context)!.partners),
              onTap: () {
                Navigator.pop(context); // chiude il drawer
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => const PartnerPage(),
                  ),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.settings),
              title: Text(AppLocalizations.of(context)!.settings),
              onTap: () {
                Navigator.pop(context); // chiude il drawer
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => const SettingsPage(),
                  ),
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.public),
              title: Text(AppLocalizations.of(context)!.website),
              onTap: () {
                Navigator.pop(context);
                _openWebsite(
                  context,
                  'https://skullnetworkitalia.zapto.org/bpt-network/',
                );
              },
            ),
            ListTile(
              leading: const Icon(Icons.bug_report),
              title: Text(
                Localizations.localeOf(context).languageCode == 'it'
                    ? 'Segnala un bug'
                    : 'Report a bug',
              ),
              onTap: () {
                Navigator.pop(context);
                _openWebsite(context, AppConstants.bugReportUrl);
              },
            ),
          ],
        ),
      ),

      // ===== CONTENUTO PAGINA =====
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            const SizedBox(height: 20),
            Image.asset(
              'assets/images/sni_logo.png',
              height: 120,
            ),
            const SizedBox(height: 20),
            Text(
              AppLocalizations.of(context)!.communityName,
              style: Theme.of(context).textTheme.headlineMedium,
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 20),
            Text(
              AppLocalizations.of(context)!.description,
              style: Theme.of(context).textTheme.bodyMedium,
              textAlign: TextAlign.center,
            ),
            const Spacer(),
            Text(
              AppLocalizations.of(context)!.copyright,
              textAlign: TextAlign.center,
              style: const TextStyle(fontSize: 12, color: Colors.grey),
            ),
          ],
        ),
      ),
    );
  }
}

Future<void> _openWebsite(BuildContext context, String url) async {
  final uri = Uri.parse(url);
  final messenger = ScaffoldMessenger.of(context);
  try {
    if (!await launchUrl(uri, mode: LaunchMode.externalApplication)) {
      messenger.showSnackBar(
        SnackBar(content: Text(AppLocalizations.of(context)!.unableOpenLink)),
      );
    }
  } catch (e) {
    messenger.showSnackBar(
      SnackBar(content: Text(AppLocalizations.of(context)!.errorOpeningLink)),
    );
  }
}
