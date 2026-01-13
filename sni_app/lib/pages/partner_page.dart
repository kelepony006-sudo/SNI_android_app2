import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';

class PartnerPage extends StatelessWidget {
  const PartnerPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Partner'),
      ),
      body: const Padding(
        padding: EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'I nostri Partner',
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
              ),
            ),

            SizedBox(height: 30),

            // ===== INSTANT GAMING =====
            PartnerCard(
              name: 'Instant Gaming',
              imageAsset: 'assets/images/instant_pannello_partner.png',
              url: 'https://www.instant-gaming.com/?igr=bitpredator',
            ),

            SizedBox(height: 20),

            // ===== ZAP HOSTING =====
            PartnerCard(
              name: 'ZAP-Hosting',
              imageAsset: 'assets/images/zap_pannello_partner.png',
              url:
                  'https://zap-hosting.com/a/0dd10586c1b68c4e7ceab6cba0ce2848ed946691?voucher=SkullNetwork-a-6091',
            ),
          ],
        ),
      ),
    );
  }
}

class PartnerCard extends StatelessWidget {
  final String name;
  final String? description;
  final String url;
  final String? imageAsset;

  const PartnerCard({
    super.key,
    required this.name,
    this.description,
    required this.url,
    this.imageAsset,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 4,
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              name,
              style: Theme.of(context).textTheme.titleLarge,
            ),
            const SizedBox(height: 12),

            InkWell(
              onTap: () async {
                final uri = Uri.parse(url);
                final messenger = ScaffoldMessenger.of(context);
                try {
                  if (!await launchUrl(uri, mode: LaunchMode.externalApplication)) {
                    messenger.showSnackBar(
                      const SnackBar(content: Text('Impossibile aprire il link')),
                    );
                  }
                } catch (_) {
                  messenger.showSnackBar(
                    const SnackBar(content: Text('Errore durante l\'apertura del link')),
                  );
                }
              },
              child: imageAsset != null
                  ? Image.asset(
                      imageAsset!,
                      height: 80,
                      fit: BoxFit.contain,
                    )
                  : Text(
                      description ?? '',
                      style: Theme.of(context)
                          .textTheme
                          .bodyMedium
                          ?.copyWith(
                            color: Colors.blue,
                            decoration: TextDecoration.underline,
                          ),
                    ),
            ),
          ],
        ),
      ),
    );
  }
}
